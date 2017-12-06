import { Vue, Component } from "av-ts";
import axios from "axios";
import $ from 'jquery';

@Component
export default class AdminPage extends Vue {
    text = "majid";
    e1 = 4;

    CheckToken(){
        axios.get('/api/Login/CheckToken', {
            params: {
               token : $("#txtToken").val()
            }
        }).then(function (response) {
            console.log(response.data);
        }.bind(this)).catch(function (error) {
            console.log(error);
        });
    }
}