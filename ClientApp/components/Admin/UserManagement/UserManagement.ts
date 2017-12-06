import { Vue, Component, Lifecycle, Watch } from "av-ts";
import axios from "axios";
import $ from 'jquery';

@Component({
    components: {
        Modal: require('../../Modal/Modal.vue')
    }
})
export default class UserApp extends Vue {
    snackbar: boolean = false;
    y: string = 'top';
    x: string = 'left';
    mode: string = '';
    timeout: number = 4000;
    Message: string = '';
    context: string = '';

    UserName: string = "";
    Password: string = "";
    UserStatus: string = "Enable";
    Description: string = "";
    UserType: string = "User";
    userID: number = -1;
    infoDiv: boolean = true;

    headers = [
        { text: 'ردیف', value: 'rowNumber', align: 'center', sortable: true },
        { text: 'نام کاربری', value: 'userName', align: 'center' },
        { text: 'نوع', value: 'userType', align: 'center' },
        { text: 'وضعیت', value: 'userStatus', align: 'center' },
        { text: 'تاریخ ایجاد', value: 'createDate', align: 'center' },
        { text: 'توضیحات', value: 'description', align: 'center' },
        { text: 'ویرایش', value: 'Edit', align: 'center' },
        { text: 'حذف', value: 'Delete', align: 'center' }
    ];

    items = [];

    @Lifecycle mounted() {
        this.GetData();
    }

    GetData() {
        // var data = axios.get('/api/User/SelectAll');
        axios.get('/api/User/SelectAll')
            .then(function (response) {
                this.items = response.data;
                for (var i = 0; i < this.items.length; i++) {
                    this.items[i].rowNumber = i + 1;
                }
                console.log(this.items);
            }.bind(this))
            .catch(function (error) {
                console.log(error);
            });
    }

    RegisterUser(_infoDiv) {
        if ((this.UserName == '' || this.Password == '') && this.userID == -1) {
            this.Message = 'باکس های ستاره دار را تکمیل نمایید';
            this.context = "error";
            this.snackbar = true;
            return;
        }
        var User = {
            UserPassword: this.Password,
            StatusCode: this.UserStatus,
            Description: this.Description,
            UserType: this.UserType,
            UserName: this.UserName,
        };
        var _url = '';
        if (this.userID == -1)
            _url = '/api/User/AddUser';
        else {
            _url = `/api/User/UpdateUser?userID=${this.userID}&Pass=${!this.infoDiv}`;
        }
        axios({
            method: 'post',
            url: _url,
            data: User
        }).then(function (response) {
            // console.log(response.data);
            this.Message = response.data.message;
            this.context = response.data.context;
            this.snackbar = true;
            if (this.context == "success")
                this.GetData();
        }.bind(this));
    }

    NewUser() {
        this.clearData();
        $("#txtUserName").focus();
    }

    clearData() {
        this.UserName = this.Password = this.Message = this.Description = '';
        this.UserStatus = 'Enable';
        this.UserType = 'User';
        this.userID = -1;
        this.snackbar = false;
        $("#userRadio").prop('checked', true);
        $("#enableRadio").prop('checked', true);
        this.infoDiv = true;
    }

    EditUser(User) {
        this.userID = User.userID;
        this.UserStatus = User.statusCode == 1 ? 'Enable' : 'Disable'
        this.Description = User.description;
        this.UserType = User.userType == 1 ? 'Admin' : 'User';
        this.UserName = User.userName;
        this.Password = "";

        if (User.statusCode == 1) {
            $("#enableRadio").prop('checked', true);
        }
        else {
            $("#disableRadio").prop('checked', true);
        }

        if (User.userType == 1) {
            $("#adminRadio").prop('checked', true);
        }
        else {
            $("#userRadio").prop('checked', true);
        }
    }

    showModal() {
        this.clearData();
    }

    DeleteUser(userCode) {
        if (confirm("آیا اطلاعات حذف شوند؟") == true) {
            axios({
                method: 'delete',
                url: `/api/User/deleteUser?UserCode=${userCode}`
            }).then(function (response) {
                console.log(response.data);
                this.Message = response.data.message;
                this.context = response.data.context;
                this.snackbar = true;
                if (this.context == 'success')
                    this.GetData();
            }.bind(this));
        }
    }
}