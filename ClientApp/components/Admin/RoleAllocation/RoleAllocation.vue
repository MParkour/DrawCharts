<template>
    <div class="col-xs-12 col-sm-10 col-sm-push-1 col-md-8 col-md-push-2" style="font-family:vazir;">
        <div style="direction:rtl; margin-top:10px; float:right;">
            <span style="float:right; margin-left:10px; margin-top:8px;">نام کاربری :</span>
            <select @change="GetUserTemp(UserSelected)" class="UserCombo" v-model="UserSelected">
                <option value="-1" selected>انتخاب نمایید</option>
                <option v-for="user in UserList" :key="user" :value="user.userID">{{user.userName}}</option>
            </select>
            <v-icon style="color:red; font-size:10pt; margin-top:10px;" v-show="UserSelected==-1">star</v-icon>
        </div>
        <v-btn @click="Register" dark style="background-color:rgb(21, 160, 199); float:left; margin:0; margin-top:10px; box-shadow: 3px 3px 5px #888888; border-radius:5px;">ثبت</v-btn>
        <v-btn @click="Clear" dark style="background-color:rgb(21, 160, 199); float:left; margin:0; margin-top:10px; margin-left:15px; box-shadow: 3px 3px 5px #888888; border-radius:5px;">جدید</v-btn>
        <v-data-table :headers="headers" hide-actions :items="TempList" v-model="selected" selected-key="tempName" select-all class="elevation-2 Roletbl">
            <template slot="headerCell" scope="props">
                <span>
                    {{ props.header.text }}
                </span>
            </template>
            <template slot="items" scope="props">
                <td>
                    <v-checkbox primary hide-details v-model="props.selected"></v-checkbox>
                </td>
                <td>{{ props.item.rowNumber }}</td>
                <td>{{ props.item.tempName }}</td>
            </template>
        </v-data-table>
        <v-snackbar style="z-index:1050;" :error="context === 'error'" :success="context === 'success'" :warning="context === 'warning'" :info="context === 'info'" :timeout="timeout" :top="y === 'top'" :bottom="y === 'bottom'" :right="x === 'right'" :left="x === 'left'" :multi-line="mode === 'multi-line'" :vertical="mode === 'vertical'" v-model="snackbar">
            <span>{{ Message }}</span>
            <v-btn fab small flat style="margin:0px; color:white;" @click.native="snackbar = false">
                <v-icon>mdi-close</v-icon>
            </v-btn>
        </v-snackbar>
    </div>
</template>

<script src="./RoleAllocation.ts"></script>
<style src="./RoleAllocation.css">