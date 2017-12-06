<template>
    <div class="col-xs-12 col-lg-10 col-lg-push-1" style="font-family:vazir;">
        <v-data-table v-bind:headers="headers" style="border-radius:10px;" :items="items" dark class="elevation-1">
            <template slot="headerCell" scope="props">
                <span>
                    {{ props.header.text }}
                </span>
            </template>
            <template slot="items" scope="props">
                <td class="text-xs-center">{{ props.item.rowNumber }}</td>
                <td class="text-xs-center">{{ props.item.userName }}</td>
                <td class="text-xs-center">{{ props.item.userType== 1 ? 'مدیر' : 'کاربر' }}</td>
                <td class="text-xs-center">{{ props.item.statusCode==1 ? 'فعال' : 'غیر فعال' }}</td>
                <td class="text-xs-center">{{ props.item.createDate }}</td>
                <td class="text-xs-center">{{ props.item.description }}</td>
                <td class="text-xs-center">
                    <v-btn fab dark small @click="EditUser(props.item)" class="teal" data-toggle="modal" data-target="#ModalForm">
                        <v-icon dark>edit</v-icon>
                    </v-btn>
                </td>
                <td class="text-xs-center">
                    <v-btn fab dark small class="error" @click="DeleteUser(props.item.userID)">
                        <v-icon dark>delete</v-icon>
                    </v-btn>
                </td>
            </template>
        </v-data-table>

        <modal>
            <div slot="ModalHeader">
                <v-btn fab dark small class="error right" style="margin:10px;" data-dismiss="modal">
                    <v-icon dark>close</v-icon>
                </v-btn>
                <p style="text-align:center; padding-top:20px;">ثبت کاربر</p>
            </div>
            <div slot="ModalBody" style="direction:rtl;">
                <div v-show="userID!=-1" style="text-align:right; padding:5px; font-family:vazir;">
                    <label style="width:120px; cursor:pointer; text-align:center;" @click="infoDiv=true">اطلاعات</label>
                    <label style="width:120px; cursor:pointer; text-align:center;" @click="infoDiv=false">کلمه عبور</label>
                </div>

                <table style="direction:rtl;">
                    <tr v-show="infoDiv">
                        <td>نام کاربری</td>
                        <td>
                            <input id="txtUserName" v-model="UserName" style="border:1px solid gray; margin-bottom:10px; margin-right:5px;" />
                        </td>
                        <td>
                            <v-icon v-show="!UserName" style="font-size:8pt; margin-top:-10px; color:red;">star</v-icon>
                        </td>
                    </tr>
                    <tr v-show="userID==-1 || !infoDiv">
                        <td>کلمه عبور</td>
                        <td>
                            <input id="txtPassword" v-model="Password" style="border:1px solid gray; margin-bottom:10px; margin-right:5px;" />
                        </td>
                        <td>
                            <v-icon v-show="!Password" style="font-size:8pt; margin-top:-10px; color:red;">star</v-icon>
                        </td>
                    </tr>
                    <tr v-show="infoDiv">
                        <td>توضیحات</td>
                        <td>
                            <input v-model="Description" style="border:1px solid gray; margin-bottom:10px; margin-right:5px;" />
                        </td>
                        <td></td>
                    </tr>
                    <tr v-show="infoDiv">
                        <td>وضعیت</td>
                        <td style="margin-bottom:10px; margin-right:5px;">
                            <label checked style="cursor:pointer;">فعال
                                <input @click="()=> UserStatus='Enable'" type="radio" name="Status" id="enableRadio" checked>
                            </label>
                            <label style="cursor:pointer;">غیر فعال
                                <input @click="()=> UserStatus='Disable'" type="radio" name="Status" id="disableRadio" />
                            </label>
                        </td>
                    </tr>
                    <tr v-show="infoDiv">
                        <td>نوع کاربر</td>
                        <td>
                            <label style="cursor:pointer;">کاربر عادی
                                <input @click="()=> UserType='User'" type="radio" name="Type" checked id="userRadio">
                            </label>
                            <label checked style="cursor:pointer;">کاربر مدیر
                                <input @click="()=> UserType='Admin'" type="radio" name="Type" id="adminRadio">
                            </label>
                        </td>
                    </tr>
                </table>
            </div>
            <div slot="ModalFooter" style="direction:rtl;">
                <v-btn round dark medium class="indigo" @click="RegisterUser(infoDiv)">
                    <span style="font-family:vazir; font-size:12pt;">{{userID==-1 ? 'ثبت' : 'ویرایش'}}</span>
                </v-btn>
                <v-btn round dark medium class="indigo" @click="NewUser">
                    <span style="font-family:vazir; font-size:12pt;">جدید</span>
                </v-btn>
            </div>
        </modal>

        <v-btn @click="showModal" fab dark medium style="float:right;" class="indigo" data-toggle="modal" data-target="#ModalForm">
            <v-icon dark>add</v-icon>
        </v-btn>

        <v-snackbar style="z-index:1050;" :error="context === 'error'" :success="context === 'success'" :warning="context === 'warning'" :info="context === 'info'" :timeout="timeout" :top="y === 'top'" :bottom="y === 'bottom'" :right="x === 'right'" :left="x === 'left'" :multi-line="mode === 'multi-line'" :vertical="mode === 'vertical'" v-model="snackbar">
            <span>{{ Message }}</span>
            <v-btn fab small flat style="margin:0px; color:white;" @click.native="snackbar = false">
                <v-icon>mdi-close</v-icon>
            </v-btn>
        </v-snackbar>
        <!-- <v-btn block primary @click.native="snackbar = true" dark>Show Snackbar</v-btn> -->
    </div>
</template>

<script src='./UserManagement.ts'></script>

<style src='./UserManagement.css' scoped>