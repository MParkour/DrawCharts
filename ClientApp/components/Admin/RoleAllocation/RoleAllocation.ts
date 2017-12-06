import { Vue, Component, Lifecycle } from "av-ts";
import axios from "axios";

@Component
export default class RoleAllocation extends Vue {
    /*Data*************************************************************** */
    UserSelected: Number = -1;
    UserList = [];

    // TempList = [];
    /*Data*************************************************************** */

    /*Data Table********************************************************* */
    selected = [];
    headers = [
        { text: 'ردیف', value: 'rowNumber' },
        { text: 'نام الگو', value: 'TempID' },
    ];
    TempList = [];
    /*Data Table********************************************************* */

    /*snackbar*********************************************************** */
    Message: string = "";
    context: string = "";
    snackbar: boolean = false;
    y: string = 'top';
    x: string = 'left';
    mode: string = '';
    timeout: number = 3000;
    /*snackbar*********************************************************** */

    /*Function*********************************************************** */
    Register() {
        if (!this.CheckData()) {
            this.Message = "لطفا کاربری را انتخاب نمایید";
            this.context = 'error';
            this.snackbar = true;
            return;
        }
        var tepmID_List = "";
        this.selected.forEach((item, index, items) => {
            tepmID_List += item.tempID + ",";
        });
        var querystring = require('querystring');
        var data = querystring.stringify({
            listTempID: tepmID_List,
            userID: this.UserSelected
        });

        axios({
            method: 'post',
            url: '/api/Role/Register',
            data: data
        }).then(function (response) {
            this.Message = response.data.message;
            this.context = response.data.context;
            this.snackbar = true;
        }.bind(this));
    }

    CheckData() {
        if (this.UserSelected == -1)
            return false;
        return true;
    }

    Clear() {
        this.UserSelected = -1;
        this.selected = [];
    }

    @Lifecycle mounted() {
        this.GetUsers();
        this.GetTemplates();
    }

    GetUsers() {
        axios.get('/api/Role/GetAllUsers')
            .then(function (response) {
                console.log(response.data);
                this.UserList = response.data;
            }.bind(this))
            .catch(function (error) {
                this.Message = "خطا در واکشی کاربران" + error;
                this.context = "error";
                this.snackbar = true;
            });
    }

    GetTemplates() {
        axios.get('/api/Role/GetAllTemplates')
            .then(function (response) {
                console.log(response.data);
                this.TempList = response.data;
                this.TempList.forEach((item, index) => {
                    item.rowNumber = index + 1;
                });
            }.bind(this))
            .catch(function (error) {
                this.Message = "خطا در واکشی الگوها" + error;
                this.context = "error";
                this.snackbar = true;
            });
    }

    GetUserTemp(userID) {
        if (userID == -1) {
            this.selected = [];
            return;
        }
        axios.get(`/api/Role/GetUserTemp?userID=${userID}`)
            .then(function (response) {
                console.log(response.data);
                this.selected = response.data;
            }.bind(this))
            .catch(function (error) {
                this.Message = "خطا در واکشی الگوهای کاربر" + error;
                this.context = "error";
                this.snackbar = true;
            });
    }
    /*Function*********************************************************** */
}