import { Vue, Component, Lifecycle } from "av-ts";
import Chart from 'chart.js';
import axios from "axios";

@Component({
    components: {
        ChartApp: require('../Chart/Chart.vue')
    }
})
export default class UserPage extends Vue {
    UserRole = [];
    ChartData: Array<number> = [];
    ChartLabel: Array<number> = [];
    ChartTitle: String = "رسم نمودار";
    tempID: number = -1;

    //search
    check1: boolean = false;
    check2: boolean = false;
    baze1: boolean = true;
    baze2: boolean = true;
    from1: string = ""; from2: string = "";
    until1: string = ""; until2: string = "";
    area1: string = ""; area2: string = "";
    strFilter1: string = ""; strFilter2: string = "";
    splitChar: string = ',';
    states = [
        'Alabama', 'Alaska', 'American Samoa', 'Arizona',
        'Arkansas', 'California', 'Colorado', 'Connecticut',
        'Delaware', 'District of Columbia', 'Federated States of Micronesia',
        'Florida', 'Georgia', 'Guam', 'Hawaii', 'Idaho',
        'Illinois', 'Indiana', 'Iowa', 'Kansas', 'Kentucky',
        'Louisiana', 'Maine', 'Marshall Islands', 'Maryland',
        'Massachusetts', 'Michigan', 'Minnesota', 'Mississippi',
        'Missouri', 'Montana', 'Nebraska', 'Nevada',
        'New Hampshire', 'New Jersey', 'New Mexico', 'New York',
        'North Carolina', 'North Dakota', 'Northern Mariana Islands', 'Ohio',
        'Oklahoma', 'Oregon', 'Palau', 'Pennsylvania', 'Puerto Rico',
        'Rhode Island', 'South Carolina', 'South Dakota', 'Tennessee',
        'Texas', 'Utah', 'Vermont', 'Virgin Island', 'Virginia',
        'Washington', 'West Virginia', 'Wisconsin', 'Wyoming'
    ];
    select = [];
    e6 = [];
    Options = [];
    //search

    //Chart Detail
    Calculation: string = "";
    xAxisLabel: string = "";
    AxisY: string = "";
    yAxisLabel: string = "";
    //Chart Detail

    /*Function**********************************************************/
    @Lifecycle mounted() {
        //نمایش الگوها
        var userID = window.localStorage.getItem("userID");
        axios.get('/api/UserPage/GetUserRole?userID=' + userID)
            .then(function (response) {
                this.UserRole = response.data;
                this.UserRole.forEach(item => {
                    item.icon = 'question_answer';
                });
                console.log(this.UserRole);
            }.bind(this))
            .catch(function (error) {
                console.log(error);
            });
    }

    DrawTemp(item) {
        //اتصال به دیتابیس خودمان و واکشی اطلاعات برای اتصال به یک دیتابیس دیگه
        this.tempID = item.tempID;
        this.ChartData = [];
        this.ChartLabel = [];
        this.ChartTitle = item.tempName;
        axios.get('/api/UserPage/GetTempData?tempID=' + this.tempID)
            .then(function (response) {
                this.xAxisLabel = response.data[0].axisX_Name;
                this.AxisY = ` ( ${response.data[0].axisY_Name} ) `;
                this.Calculation = response.data[0].calculation;
                this.yAxisLabel = this.Calculation+' ' + this.AxisY;
                response.data.forEach(item => {
                    this.ChartData.push(item.field2);
                    this.ChartLabel.push(item.field1);
                });
            }.bind(this))
            .catch(function (error) {
                console.log(error);
            });
    }

    ApplyFilter() {
        if (this.tempID == -1) {
            alert('الگویی انتخاب نشده است');
            return;
        }
        var flag: boolean = false;
        //فیلتر اول فعال باشد
        if (this.check1) {
            //فیلتر به صورت بازه است
            if (this.baze1) {
                this.strFilter1 = this.from1 + ',' + this.until1;
                // alert(this.strFilter1);
            }
            //فیلتر به صورت گزینه است
            else {
                this.strFilter1 = this.area1;
            }
            //یعنی فیلتری وجود دارد
            flag = true;
        }
        //فیلتر دوم فعال باشد
        if (this.check2) {
            //فیلتر به صورت بازه است
            if (this.baze2) {
                this.strFilter2 = this.from2 + ',' + this.until2;
                // alert(this.strFilter2);
            }
            //فیلتر به صورت گزینه است
            else {
                this.strFilter2 = this.area2;
            }
            //یعنی فیلتری وجود دارد
            flag = true;
        }
        //فیلتری برای اعمال وجود ندارد
        if (!flag) {
            //واکشی ساده اطلاعات بدون فیلتر گذاری
        }
        axios.get('/api/UserPage/SetFilter', {
            params: {
                tempID: this.tempID,
                Check1: this.check1,
                Check2: this.check2,
                Baze1: this.baze1,
                Baze2: this.baze2,
                listFilter1: this.strFilter1,
                listFilter2: this.strFilter2,
            }
        }).then(function (response) {
            this.ChartData = [];
            this.ChartLabel = [];
            response.data.forEach(item => {
                this.ChartData.push(item.field2);
                this.ChartLabel.push(item.field1);
            });
        }.bind(this))
            .catch(function (error) {
                console.log(error);
            });
    }
    /*Function**********************************************************/
}