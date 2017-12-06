<template>
    <div class="wrapper" style="font-family:vazir; font-size=12pt;">
        <div class="myContainer">
            <h1>خوش آمدید</h1>
            <div class="myform">
                <input type="text" placeholder="نام کاربری" v-model="UserName">
                <input type="password" placeholder="کلمه عبور" v-model="Password" @keypress="keyPress">
                <!-- <router-link :to="myGetter">
                    <button @click="LoginClicked">ورود</button>
                </router-link> -->
                <button @click="LoginClicked">ورود</button>
                <!-- <span v-show="false" v-text="myGetter"></span> -->
            </div>
        </div>
        
        <ul class="bg-bubbles">
            <li></li>
            <li></li>
            <li></li>
            <li></li>
            <li></li>
            <li></li>
            <li></li>
            <li></li>
            <li></li>
            <li></li>
        </ul>

        <v-snackbar :error="Context === 'error'" :warning="Context === 'warning'" :info="Context === 'info'" :timeout="timeout" :top="y === 'top'" :bottom="y === 'bottom'" :right="x === 'right'" :left="x === 'left'" :multi-line="mode === 'multi-line'" :vertical="mode === 'vertical'" v-model="snackbar">
            <span>{{ Message }}</span>
            <v-btn fab small flat style="margin:0px; color:white;" @click.native="snackbar = false">
                <v-icon>mdi-check</v-icon>
            </v-btn>
        </v-snackbar>
    </div>
</template>

<script src='./LoginPage.ts'></script>

<style src="./LoginPage.css" scoped>