<template>
    <div id='app-root'>
        <v-app top-toolbar footer style="background-color:#eaeaea;">
            <main>
                <!-- <v-content> -->
                    <v-container fluid>
                        <input id="txtToken" v-show="false" />
                        <router-view></router-view>
                    </v-container>
                <!-- </v-content> -->
            </main>
        </v-app>
    </div>
</template>

<script src="./app.ts"></script>
