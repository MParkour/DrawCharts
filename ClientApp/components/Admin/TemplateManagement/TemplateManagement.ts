import { Vue, Component, Watch, Lifecycle } from "av-ts";
import axios from "axios";
import $ from 'jquery';

@Component({
  components: {
    Modal: require('../../Modal/Modal.vue')
  }
})
export default class TemplateManagement extends Vue {
  /*data************************************************************ */
  TempName: string = "";
  IP: string = "";
  UserName: string = "";
  Password: string = "";
  DbName: string = "";
  TblName: string = "";
  Field1: string = "";
  Field2: string = "";
  Calculation: string = "";
  isConnected: boolean = false;
  fileType: string = "";
  TableList = [];
  FieldList1 = [];
  FieldList2 = [];
  ClacList = ["Sum", "Count", "Average", "Max", "Min"];
  DBType: string = "";
  /*data************************************************************ */

  /*watch*********************************************************** */
  @Watch('TempName')
  TempNamehandler(newVal, oldVal) {
    this.snackbar = false;
  }
  @Watch('IP')
  IPhandler(newVal, oldVal) {
    this.ClearWatch();
  }
  @Watch('UserName')
  UserNamehandler(newVal, oldVal) {
    this.ClearWatch();
  }
  @Watch('Password')
  Passwordhandler(newVal, oldVal) {
    this.ClearWatch();
  }
  @Watch('DbName')
  DbNamehandler(newVal, oldVal) {
    this.ClearWatch();
  }
  @Watch('fileType')
  fileTypehandler(newVal, oldVal) {
    this.ClearWatch();
  }
  @Watch('TblName')
  TblNamehandler(newVal, oldVal) {
    this.snackbar = false;
  }
  @Watch('Field1')
  Field1handler(newVal, oldVal) {
    this.snackbar = false;
  }
  @Watch('Field2')
  Field2handler(newVal, oldVal) {
    this.snackbar = false;
  }
  @Watch('Calculation')
  Calculationhandler(newVal, oldVal) {
    this.snackbar = false;
  }
  ClearWatch() {
    this.Field1 = "";
    this.Field2 = "";
    this.TblName = "";
    this.Calculation = "";
    this.FieldList2 = [];
    this.FieldList1 = [];
    this.TableList = [];

    this.isConnected = false;
    this.snackbar = false;
  }
  /*watch*********************************************************** */

  /*DataTable******************************************************* */
  pagination = {
    rowsPerPage: 5
  };
  selected = [];
  headers = [
    { text: 'ردیف', value: 'rowNumber', align: 'center', sortable: true },
    { text: 'نام الگو', value: 'tempName', align: 'center' },
    { text: 'آدرس', value: 'IP', align: 'center' },
    { text: 'نام کاربری', value: 'userName', align: 'center' },
    { text: 'کلمه عبور', value: 'password', align: 'center' },
    { text: 'نام جدول', value: 'tblName', align: 'center' },
    { text: 'فیلد اول', value: 'field1', align: 'center' },
    { text: 'فیلد دوم', value: 'field2', align: 'center' },
    { text: 'حذف', value: 'Delete', align: 'center' }
  ];
  items = [];
  get pages() {
    return this.pagination.rowsPerPage ? Math.ceil(this.items.length / this.pagination.rowsPerPage) : 0
  }
  /*DataTable******************************************************* */


  /*snackbar******************************************************** */
  Message: string = "";
  context: string = "";
  snackbar: boolean = false;
  y: string = 'top';
  x: string = 'left';
  mode: string = '';
  timeout: number = 3000;
  /*snackbar******************************************************** */


  /*fuction********************************************************* */
  RegisterTemplate() {
    if (!this.isConnected) {
      this.Message = "اتصال خود را بررسی نمایید";
      this.context = "error";
      this.snackbar = true;
      return;
    }
    if (!this.checkData(2)) return;
    var template = {
      Title: this.TempName,
      IP: this.IP,
      UserName: this.UserName,
      Password: this.Password,
      dbName: (this.DBType.toLocaleLowerCase() == "sqlite" ? this.DbName + this.fileType : this.DbName),
      TableName: this.TblName,
      Field1: this.Field1,
      Field2: this.Field2,
      Calculation: this.Calculation,
      dbType: this.DBType
    }

    axios({
      method: 'post',
      url: '/api/Template/RegisterTemplate',
      data: template
    }).then(function (response) {
      this.Message = response.data.message;
      this.context = response.data.context;
      this.snackbar = true;
      if (this.context == "success")
        this.selectAll();
    }.bind(this)).catch(function (error) {
      this.Message = "خطای برنامه" + error;
      this.context = "error";
      this.snackbar = true;
    });
  }

  DeleteTemplate(templateID) {
    if (confirm("آیا اطلاعات حذف شوند؟") == true) {
      axios({
        method: 'delete',
        url: `/api/Template/deleteTemplate?TemplateID=${templateID}`
      }).then(function (response) {
        this.Message = response.data.message;
        this.context = response.data.context;
        this.snackbar = true;
        if (this.context == 'success')
          this.selectAll();
      }.bind(this)).catch(function (error) {
        this.Message = "خطای برنامه" + error;
        this.context = "error";
        this.snackbar = true;
      });
    }
  }

  NewTemplate() {
    this.clearData();
  }

  clearData() {
    this.TempName = this.IP = this.UserName = this.Password = this.DbName = "";
    //خالی کردن سه کامبو باکس
    $("#txtTempName").focus();
  }

  showModal() {
  }

  CheckConnection() {
    //بررسی فیلدها
    this.setConnectionValue();
    if (!this.checkData(1)) return;

    //ارسال درخواست به سرور مورد نظر برای اینکه ببیند آماده و موجود هست یا نه
    // var ConnectionInfo = {
    //   IP: this.IP,
    //   userName: this.UserName,
    //   password: this.Password,
    //   dbName: this.DbName,
    //   fileType : this.fileType
    // }

    var data = `${this.DBType},${this.IP},${this.UserName},${this.Password},${this.DbName},${this.fileType}`;
    // this.ConnectionString = `Data Source=\\${this.IP}\\${this.DbName}${this.fileType}`;

    //Send Request
    axios.get('/api/Template/CheckConnection', {
      params: {
        data: data
      }
    }).then(function (response) {
      this.TableList = response.data.tableList;
      this.Message = response.data.message;
      this.context = response.data.context;
      this.snackbar = true;
      if (this.context == "success") {
        this.isConnected = true;
      }
    }.bind(this)).catch(function (error) {
      this.Message = "خطای برنامه" + error;
      this.context = "error";
      this.snackbar = true;
    });
  }

  checkData(code) {
    var flag = false;
    if (code == 1) {
      if (this.TempName == "" || this.IP == "" || this.DbName == "") {
        flag = true;
      }
    }
    else if (code == 2)
      if ((this.TblName == "" || this.Field1 == "" || this.Field2 == "" || this.Calculation == "") && this.isConnected) {
        flag = true;
      }
    if (flag) {
      this.Message = "ابتدا فیلدهای ستاره دار را تکمیل نمایید";
      this.context = 'error';
      this.snackbar = true;
      return false;
    }
    return true;
  }

  setConnectionValue() {
    this.TempName = this.TempName.trim() ? this.TempName : "";
    this.IP = this.IP.trim() ? this.IP : "";
    this.UserName = this.UserName.trim() ? this.UserName : "";
    this.Password = this.Password.trim() ? this.Password : "";
    this.DbName = this.DbName.trim() ? this.DbName : "";
  }

  selectAll() {
    axios.get('/api/Template/SelectAll')
      .then(function (response) {
        this.items = response.data;
        for (var i = 0; i < this.items.length; i++) {
          this.items[i].rowNumber = i + 1;
        }
      }.bind(this))
      .catch(function (error) {
        this.Message = "خطای برنامه" + error;
        this.context = "error";
        this.snackbar = true;
      });
  }

  @Lifecycle mounted() {
    this.selectAll();
  }

  TblNameChanged() {
    this.GetTableFields();
  }

  GetTableFields() {
    this.Field1 = "";
    this.Field2 = "";
    this.FieldList2 = [];
    this.FieldList1 = [];
    if (this.TblName == "")
      return;
    var data = `${this.DBType},${this.IP},${this.UserName},${this.Password},${this.DbName},${this.fileType}`;
    axios.get('/api/Template/GetTableField', {
      params: {
        data: data,
        tableName: this.TblName
      }
    }).then(function (response) {
      this.FieldList1 = response.data.fieldList;
    }.bind(this)).catch(function (error) {
      this.Message = "خطای برنامه" + error;
      this.context = "error";
      this.snackbar = true;
    });
  }

  ChangeField2() {
    // this.FieldList2 = this.FieldList1.filter(item => item != this.Field1);
    this.FieldList2 = this.FieldList1;
    this.Field2 = "";
  }
  /*fuction********************************************************* */
}