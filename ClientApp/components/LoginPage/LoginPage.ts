import { Vue, Component, Lifecycle } from "av-ts";
import axios from "axios";
import $ from "jquery";

@Component()
export default class LoginPage extends Vue {
    Password: string = "";
    UserName: string = "";

    Message: string = "";
    Context: string = 'success';

    snackbar = false;
    y = 'top';
    x = 'left';
    mode = '';
    timeout = 3000;

    LoginClicked() {
        this.Login(this.UserName, this.Password);
    }

    keyPress(event) {
        if (event.keyCode == 13)
            this.Login(this.UserName, this.Password);
    }

    Login(_userName, _userPassword) {
        if (_userName == "" || _userPassword == "") {
            this.Message = "نام کاربری یا کلمه عبور نمی تواند خالی باشد";
            this.Context = "error";
            this.snackbar = true;
            return;
        }
        axios.get('/api/Login/signIn', {
            params: {
                UserName: _userName,
                Password: _userPassword
            }
        }).then(function (response) {
            console.log(response.data);
            this.Message = response.data.message;
            this.Context = response.data.context;
            if (this.Context != "error") {
                $("#txtToken").val(response.data.token);
                // window.localStorage.setItem("1","majid");
                this.$router.push(response.data.page);
                window.localStorage.setItem("userID", response.data.userID);
            }
            else
                this.snackbar = true;
        }.bind(this)).catch(function (error) {
            console.log(error);
        });
    }
}