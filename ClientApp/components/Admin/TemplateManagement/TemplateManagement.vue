<template>
    <div class="col-xs-12 col-md-8 col-md-push-2 col-lg-12 col-lg-push-0" style="font-family:vazir;">
        <v-data-table v-bind:headers="headers" v-bind:items="items" v-bind:pagination.sync="pagination" hide-actions class="elevation-1 table1">
            <template slot="headerCell" scope="props">
                <span>
                    {{ props.header.text }}
                </span>
            </template>
            <template slot="items" scope="props">
                <td>{{ props.item.rowNumber }}</td>
                <td class="text-xs-right">{{ props.item.title }}</td>
                <td class="text-xs-right" style="direction:ltr;">{{ props.item.ip }}</td>
                <td class="text-xs-right">{{ props.item.userName }}</td>
                <td class="text-xs-right">{{ props.item.password }}</td>
                <td class="text-xs-right">{{ props.item.tableName }}</td>
                <td class="text-xs-right">{{ props.item.field1 }}</td>
                <td class="text-xs-right">{{ props.item.field2 }}</td>
                <td class="text-xs-center">
                    <v-btn class="error" fab dark small style="background-color:#848484;" @click="DeleteTemplate(props.item.templateID)">
                        <v-icon dark>delete</v-icon>
                    </v-btn>
                </td>
            </template>
        </v-data-table>
        <div class="text-xs-center" style="border-radius:0px 0px 10px 10px; padding:10px;">
            <v-pagination v-model="pagination.page" :length="pages"></v-pagination>
        </div>

        <v-btn @click="showModal" fab dark medium style="float:right;" class="indigo" data-toggle="modal" data-target="#ModalForm">
            <v-icon dark>add</v-icon>
        </v-btn>

        <modal>
            <div slot="ModalHeader">
                <v-btn fab dark small class="error" data-dismiss="modal" style="float:right; margin:10px;">
                    <v-icon dark>close</v-icon>
                </v-btn>
                <p style="text-align:center; padding-top:20px;">ثبت الگو</p>
            </div>
            <div slot="ModalBody" style="direction:rtl;">
                <table style="direction:rtl;">
                    <tr>
                        <td>نام الگو</td>
                        <td>
                            <input id="txtTempName" v-model="TempName" style="border:1px solid gray; margin-bottom:10px; margin-right:5px;" />
                        </td>
                        <td>
                            <v-icon v-show="!TempName" style="font-size:8pt; margin-top:-10px; color:red;">star</v-icon>
                        </td>
                    </tr>
                    <tr>
                        <td>آدرس مقصد</td>
                        <td>
                            <input id="txtIP" placeholder="192.168.12.1" v-model="IP" style="direction:ltr; border:1px solid gray; margin-bottom:10px; margin-right:5px;" />
                        </td>
                        <td>
                            <v-icon v-show="!IP" style="font-size:8pt; margin-top:-10px; color:red;">star</v-icon>
                        </td>
                    </tr>
                    <tr>
                        <td>نام کاربری</td>
                        <td>
                            <input v-model="UserName" style="border:1px solid gray; margin-bottom:10px; margin-right:5px;" />
                        </td>
                        <td>
                            <!-- <v-icon v-show="!UserName" style="font-size:8pt; margin-top:-10px; color:red;">star</v-icon> -->
                        </td>
                    </tr>
                    <tr>
                        <td>کلمه عبور</td>
                        <td>
                            <input v-model="Password" style="border:1px solid gray; margin-bottom:10px; margin-right:5px;" />
                        </td>
                        <td>
                            <!-- <v-icon v-show="!Password" style="font-size:8pt; margin-top:-10px; color:red;">star</v-icon> -->
                        </td>
                    </tr>
                    <tr>
                        <td>نام دیتابیس</td>
                        <td>
                            <input v-model="DbName" style="border:1px solid gray; margin-bottom:10px; margin-right:5px;" />
                        </td>
                        <td>
                            <v-icon v-show="!DbName" style="font-size:8pt; margin-top:-10px; color:red;">star</v-icon>
                        </td>
                    </tr>
                    <tr>
                        <td>نوع دیتابیس</td>
                        <td>
                            <select v-model="DBType" style="min-width:130px;">
                                <option value="" selected>انتخاب نمایید</option>
                                <option value="SQLSERVER">SQL SERVER</option>
                                <option value="Sqlite">Sqlite</option>
                            </select>
                        </td>
                        <td>
                            <v-icon v-show="!DBType" style="font-size:8pt; margin-top:-10px; color:red;">star</v-icon>
                        </td>
                    </tr>
                    <tr>
                        <td>پسوند فایل</td>
                        <td>
                            <select v-model="fileType" style="min-width:130px;">
                                <option value="" selected>انتخاب نمایید</option>
                                <option value=".db">db.</option>
                                <option value=".sqlite">sqlite.</option>
                            </select>
                        </td>
                        <td>
                            <v-icon v-show="!fileType" style="font-size:8pt; margin-top:-10px; color:red;">star</v-icon>
                        </td>
                    </tr>
                    <tr v-if="isConnected">
                        <td>نام جدول</td>
                        <td>
                            <select v-model="TblName" @change="TblNameChanged" style="min-width:130px;">
                                <option value="" selected>انتخاب نمایید</option>
                                <option v-for="item in TableList" :key="item" :value="item">{{item}}</option>
                            </select>
                        </td>
                        <td>
                            <v-icon v-show="!TblName" style="font-size:8pt; margin-top:-10px; color:red;">star</v-icon>
                        </td>
                    </tr>
                    <tr v-if="isConnected">
                        <td>فیلد اول</td>
                        <td>
                            <select @change="ChangeField2" v-model="Field1" style="min-width:130px;">
                                <option value="" selected>انتخاب نمایید</option>
                                <option v-for="item in FieldList1" :key="item" :value="item">{{item}}</option>
                            </select>
                        </td>
                        <td>
                            <v-icon v-show="!Field1" style="font-size:8pt; margin-top:-10px; color:red;">star</v-icon>
                        </td>
                    </tr>
                    <tr v-if="isConnected">
                        <td>فیلد دوم</td>
                        <td>
                            <select v-model="Field2" style="min-width:130px;">
                                <option value="" selected>انتخاب نمایید</option>
                                <option v-for="item in FieldList2" :key="item" :value="item">{{item}}</option>
                            </select>
                        </td>
                        <td>
                            <v-icon v-show="!Field2" style="font-size:8pt; margin-top:-10px; color:red;">star</v-icon>
                        </td>
                    </tr>
                    <tr v-if="isConnected">
                        <td>نحوه محاسبه</td>
                        <td>
                            <select v-model="Calculation" style="min-width:130px;">
                                <option value="" selected>انتخاب نمایید</option>
                                <option v-for="item in ClacList" :key="item" :value="item">{{item}}</option>
                            </select>
                        </td>
                        <td>
                            <v-icon v-show="!Calculation" style="font-size:8pt; margin-top:-10px; color:red;">star</v-icon>
                        </td>
                    </tr>
                </table>
            </div>
            <div slot="ModalFooter" style="direction:rtl;">
                <v-btn round dark medium :class="isConnected ? 'indigo' : 'error'" @click="CheckConnection">
                    <span style="font-family:vazir; font-size:12pt;">بررسی اتصال</span>
                </v-btn>

                <v-btn round dark medium class="indigo" @click="RegisterTemplate">
                    <span style="font-family:vazir; font-size:12pt;">ثبت</span>
                </v-btn>
                <v-btn round dark medium class="indigo" @click="NewTemplate">
                    <span style="font-family:vazir; font-size:12pt;">جدید</span>
                </v-btn>
            </div>
        </modal>

        <v-snackbar style="z-index:1050;" :error="context === 'error'" :success="context === 'success'" :warning="context === 'warning'" :info="context === 'info'" :timeout="timeout" :top="y === 'top'" :bottom="y === 'bottom'" :right="x === 'right'" :left="x === 'left'" :multi-line="mode === 'multi-line'" :vertical="mode === 'vertical'" v-model="snackbar">
            <span>{{ Message }}</span>
            <v-btn fab small flat style="margin:0px; color:white;" @click.native="snackbar = false">
                <v-icon>mdi-close</v-icon>
            </v-btn>
        </v-snackbar>
    </div>
</template>

<script src='./TemplateManagement.ts'></script>

<style src="./TemplateManagement.css">

