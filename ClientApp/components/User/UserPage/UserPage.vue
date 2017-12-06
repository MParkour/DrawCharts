<template>
    <div>
        <v-toolbar absolute style="background-color:rgb(44, 62, 80);" class="lighten-0" dark scroll-off-screen scroll-target="#scrolling-techniques">
            <v-toolbar-side-icon></v-toolbar-side-icon>
            <v-spacer></v-spacer>
            <v-toolbar-title style="margin-right:10px; font-family:vazir; font-size:13pt;">صفحه کاربران عادی</v-toolbar-title>
        </v-toolbar>
        <div style="width:15%; height:500px; float:right; direction:rtl;">
            <!-- <v-btn @click="DrawChart" dark style="background-color:rgb(21, 160, 199); float:left; margin:0; margin-top:10px; margin-left:15px; box-shadow: 3px 3px 5px #888888; border-radius:5px;">رسم نمودار</v-btn> -->
            <p class="myTempTitle">الگوهای من</p>
            <v-list dense class="pt-0">
                <v-list-tile v-for="item in UserRole" :key="item">
                    <v-list-tile-action @click="DrawTemp(item)">
                        <v-icon style="color: rgb(255, 174, 20) !important;">{{ item.icon }}</v-icon>
                    </v-list-tile-action>
                    <v-list-tile-content @click="DrawTemp(item)">
                        <v-list-tile-title style="text-align: right; color: black; font-family:vazir; font-size:10pt;">{{ item.tempName }}</v-list-tile-title>
                    </v-list-tile-content>
                </v-list-tile>
            </v-list>
        </div>
        <div style="float:right; width:85%; height:100%; border-bottom: 1px solid gray; border-right:1px solid gray;">
            <!-- <div class="ChartDetails">
                        <p>راهنمای نمودار</p>
                        <p>{{xAxisLabel}} = x محور</p>
                        <p>{{Calculation}}{{AxisY}} = y محور</p>
                    </div> -->
            <div style="width:75%; margin:20px auto 30px;">
                <chart-app :xAxis="xAxisLabel" :yAxis="yAxisLabel" :ChartData="ChartData" :ChartLabel="ChartLabel" :ChartTitle="ChartTitle"></chart-app>
            </div>
            <div class="SearchBox">
                <p class="FilterTitle defaultCursor">
                    فیلتر گذاری
                </p>
                <div class="searchBoxItem">
                    <div style="float:right;">
                        <label class="pointerCursor"><input type="checkbox" v-model="check1"> فیلتر محور افقی</label>
                    </div>
                    <div style="margin-right:59px; float:right;">
                        <label>نحوه جستجو :</label>
                        <label class="pointerCursor"><input type="radio" name="searchType1" @click="baze1 = true" checked> بازه</label>
                        <label class="pointerCursor"><input type="radio" name="searchType1" @click="baze1 = false"> گزینه</label>
                    </div>

                    <div v-show="baze1" style="margin-right:50px; float:right;">
                        <label>از :</label>
                        <input v-model="from1" style="border-bottom:1px solid gray; text-align:center; width:100px;">
                        <label style="margin-right:5px;">تا :</label>
                        <input v-model="until1" style="border-bottom:1px solid gray; text-align:center; width:100px;">
                    </div>

                    <div v-show="!baze1">
                        <textarea v-model="area1"></textarea>
                    </div>
                    <!-- <v-select v-show="!baze1" style="width:50%; direction:rtl; right:50px; top:-22px; padding-bottom:0px;" label="انتخاب" v-bind:items="states" v-model="e6" multiple max-height="300" hint="مواردی را انتخاب نمایید" persistent-hint></v-select> -->
                </div>

                <div class="searchBoxItem">
                    <div style="float:right;">
                        <label class="pointerCursor"><input type="checkbox" v-model="check2"> فیلتر محور عمودی</label>
                    </div>
                    <div style="margin-right:50px; float:right;">
                        <label>نحوه جستجو :</label>
                        <label class="pointerCursor"><input type="radio" name="searchType2" @click="baze2 = true" checked> بازه</label>
                        <label class="pointerCursor"><input type="radio" name="searchType2" @click="baze2 = false"> گزینه</label>
                    </div>

                    <div v-show="baze2" style="margin-right:50px; float:right;">
                        <label>از :</label>
                        <input v-model="from2" style="border-bottom:1px solid gray; text-align:center; width:100px;">
                        <label style="margin-right:5px;">تا :</label>
                        <input v-model="until2" style="border-bottom:1px solid gray; text-align:center; width:100px;">
                    </div>

                    <div v-show="!baze2">
                        <textarea v-model="area2"></textarea>
                    </div>
                    <div style="float:left;">
                        <v-btn round dark small style="background-color:#088bd4;" @click="ApplyFilter">
                            <span style="font-family:vazir; font-size:9pt;">اعمال فیلتر</span>
                        </v-btn>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script src='./UserPage.ts'></script>

<style src="./UserPage.css" />
