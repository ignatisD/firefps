using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LoginMenu : MonoBehaviour
{
    ////These variables are set in the Inspector:

    //they are enabled and disabled to show and hide the different parts of the UI
    public GameObject login_object;
	public GameObject register_object;
	public GameObject loading_object;

    [SerializeField]
    string account_api = "https://games.ignatisd.gr/api.php";

    [SerializeField]
    string forgotPasswordUrl = "https://games.ignatisd.gr/forgot-password.html";

    //these are the login input fields:
    public InputField input_login_email;
	public InputField input_login_password;

    //these are the register input fields:
    public InputField input_register_email;
    public InputField input_register_username;
	public InputField input_register_password;
	public InputField input_register_confirmPassword;
	
	//red error UI Texts:
	public Text login_error;
	public Text register_error;
	
	////These variables cannot be set in the Inspector:
	
	//the part of UI currently being shown
	// 0 = login, 1 = register, 2 = logged in, 3 = loading
	int part = 0;

    //scene starts showing login
    EventSystem system;

    void Start () {
        system = EventSystem.current;
        //sets error Texts string to blank
        blankErrors();
        goToPart(0);

    }

    void Update () {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            switch (part)
            {
                case 0:
                    login_login_Button();
                    break;
                case 1:
                    register_register_Button();
                    break;
            }
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {

            Selectable next;
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnUp();
            }
            else
            {
                next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();
            }

            if (next != null)
            {

                InputField inputfield = next.GetComponent<InputField>();
                if (inputfield != null) inputfield.OnPointerClick(new PointerEventData(system));  //if it's an input field, also set the text caret

                system.SetSelectedGameObject(next.gameObject, new BaseEventData(system));
            }
            //else Debug.Log("next nagivation element not found");

        }
        if(GameManager.singleton.platform != "WEB" && Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.BackQuote))
        {
            GameManager.singleton.StartLocalServer();
        }
    }

    void goToPart(int _part)
    {
        part = _part;
        switch (_part)
        {
            case 0:
                login_object.gameObject.SetActive(true);
                register_object.gameObject.SetActive(false);
                loading_object.gameObject.SetActive(false);
                system.SetSelectedGameObject(input_login_email.gameObject, new BaseEventData(system));
                break;
            case 1:
                login_object.gameObject.SetActive(false);
                register_object.gameObject.SetActive(true);
                loading_object.gameObject.SetActive(false);
                system.SetSelectedGameObject(input_register_email.gameObject, new BaseEventData(system));
                break;
            case 2:
                // We are logged in - We have already transitioned to a new scene... Hopefully!
                break;
            case 3:
                login_object.gameObject.SetActive(false);
                register_object.gameObject.SetActive(false);
                loading_object.gameObject.SetActive(true);
                break;
            default:
                login_object.gameObject.SetActive(true);
                register_object.gameObject.SetActive(false);
                loading_object.gameObject.SetActive(false);
                break;
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }

	void blankErrors () {
		//blanks all error texts when part is changed e.g. login > Register
		login_error.text = "";
		register_error.text = "";
	}
	
	public void login_Register_Button ()
    { //called when the 'Register' button on the login part is pressedpart
        goToPart(1);//show register UI
		blankErrors();
	}
	
	public void register_Back_Button () { //called when the 'Back' button on the register part is pressed
        goToPart(0); //goes back to showing login UI
        blankErrors();
	}
	
	public void data_LogOut_Button () { //called when the 'Log Out' button on the data part is pressed
        goToPart(0); //goes back to showing login UI

        blankErrors();
        UserAccountManager.singleton.LogOut();
	}

    public void login_login_Button()
    { 
      //called when the 'Login' button on the login part is pressed
      //check fields aren't blank
        if ((input_login_email.text != "") && (input_login_password.text != ""))
        {
            
            StartCoroutine(sendLoginRequest(input_login_email.text, input_login_password.text)); //calls function to send login request
            goToPart(3); //show 'loading...'

        }
        else
        {
            //one of the fields is blank so return error
            login_error.text = "Field Blank!";
            input_login_password.text = ""; //blank password field
        }

    }
    IEnumerator sendLoginRequest(string username, string password)
    {

        // Create a form object
        WWWForm form = new WWWForm();

        form.AddField("email", username);
        form.AddField("password", password);
        form.AddField("action", "login");

        // Create a download object
        WWW returned = new WWW(account_api, form);

        // Wait until the download is done
        yield return returned;

        //blank password field
        input_login_password.text = "";
        if (!string.IsNullOrEmpty(returned.error))
        {
            //Account Not Created, another error occurred
            goToPart(0); //back to login UI
            login_error.text = "Database Error. Try again later.";
            Debug.Log("Error downloading: " + returned.error);
            yield break;
        }
        Debug.Log(returned.text);
        NetworkResponse response = JsonUtility.FromJson<NetworkResponse>(returned.text);
        Dictionary<string, string> results = response.ToDictionary();

        if (results["success"] == "true" && response.user != null)
        {
            //Password was correct
            blankErrors();
            goToPart(2); //show logged in UI
            string userdata = string.IsNullOrEmpty(response.user.userdata) ? "{\"kills\":0,\"deaths\":0}" : response.user.userdata;

            //blank username field
            input_login_email.text = ""; //password field is blanked

            UserAccountManager.singleton.LogIn(response.user.email, response.user.username, password, userdata);
        }else
        {
            login_error.text = results["errors"];
            goToPart(0); //back to login UI
        }
                
    }
    

    public void register_register_Button()
    { //called when the 'Register' button on the register part is pressed
        
            //check fields aren't blank
            if ((input_register_email.text != "") && (input_register_username.text != "") && (input_register_password.text != "") && (input_register_confirmPassword.text != ""))
            {

                //check username is longer than 4 characters
                if (input_register_username.text.Length > 4)
                {

                    //check password is longer than 6 characters
                    if (input_register_password.text.Length > 6)
                    {

                        //check passwords are the same
                        if (input_register_password.text == input_register_confirmPassword.text)
                        {

                            //ready to send request
                            StartCoroutine(sendRegisterRequest(input_register_email.text, input_register_username.text, input_register_password.text, input_register_confirmPassword.text)); //calls function to send register request
                            goToPart(3); //show 'loading...'

                        }
                        else
                        {
                            //return passwords don't match error
                            register_error.text = "Passwords don't match!";
                            input_register_password.text = ""; //blank password fields
                            input_register_confirmPassword.text = "";
                        }

                    }
                    else
                    {
                        //return password too short error
                        register_error.text = "Password too Short";
                        input_register_password.text = ""; //blank password fields
                        input_register_confirmPassword.text = "";
                    }

                }
                else
                {
                    //return username too short error
                    register_error.text = "Username too Short";
                    input_register_password.text = ""; //blank password fields
                    input_register_confirmPassword.text = "";
                }

            }
            else
            {
                //one of the fields is blank so return error
                register_error.text = "Field Blank!";
                input_register_password.text = ""; //blank password fields
                input_register_confirmPassword.text = "";
            }

    }

    IEnumerator sendRegisterRequest(string email, string username, string password, string password_confirm)
    {

        // Create a form object
        WWWForm form = new WWWForm();

        form.AddField("email", email);
        form.AddField("username", username);
        form.AddField("password", password);
        form.AddField("password_confirm", password_confirm);
        form.AddField("action", "register");

        // Create a download object
        WWW returned = new WWW(account_api, form);

        // Wait until the download is done
        yield return returned;

        //blank password fields
        input_register_password.text = "";
        input_register_confirmPassword.text = "";

        if (!string.IsNullOrEmpty(returned.error))
        {
            //Account Not Created, another error occurred
            goToPart(1);  //back to register UI
            register_error.text = "Database Error. Try again later.";
            Debug.Log("Error downloading: " + returned.error);
            yield break;
        }
     //   Debug.Log(returned.text);
        NetworkResponse response = JsonUtility.FromJson<NetworkResponse>(returned.text);
        Dictionary<string, string> results = response.ToDictionary();

        if (results["success"] == "true" && response.user != null)
        {
            //Account created successfully

            blankErrors();
            goToPart(2);  //show logged in UI

            string userdata = string.IsNullOrEmpty(response.user.userdata) ? "{\"kills\":0,\"deaths\":0}" : response.user.userdata;
            //blank username field
            input_register_username.text = "";

            UserAccountManager.singleton.LogIn(response.user.email, response.user.username, password, userdata);
        }
        else
        {
            register_error.text = results["errors"];
            goToPart(1);  //back to register UI
        }


    }


    public void ForgotPassword()
    {
        Application.OpenURL(forgotPasswordUrl);
    }
}
