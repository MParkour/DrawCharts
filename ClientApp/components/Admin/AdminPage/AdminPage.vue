    <template>
    <div>
        <v-toolbar absolute style="background-color:rgb(44, 62, 80);" class="lighten-0" dark scroll-off-screen scroll-target="#scrolling-techniques">
            <v-toolbar-side-icon></v-toolbar-side-icon>
            <v-spacer></v-spacer>
            <v-toolbar-title style="margin-right:10px; font-family:vazir; font-size:13pt;">خوش آمدید</v-toolbar-title>
        </v-toolbar>
        <v-card height="0" style="background-color:whitesmoke;">
            <v-bottom-nav absolute value="true" style="background-color:rgb(52, 73, 94); box-shadow:0 8px 4px -1px rgba(0,0,0,.2), 0 8px 5px rgba(0,0,0,.14), 0 1px 10px rgba(0,0,0,.12); height:75px; top:0;">
                <router-link to="/AdminPage/RoleAllocation">
                    <v-btn flat light @click.native="e1 = 1" :value="e1 === 1" style="height:75px;">
                        <span style="color:white; font-family:'vazir'; margin-top:-8px;">تخصیص الگو</span>
                        <v-icon style="color:white; font-size:28pt; margin-top:-12px;">mdi-security</v-icon>
                    </v-btn>
                </router-link>
                <router-link to="/AdminPage/TemplateManagement">
                    <v-btn flat light @click.native="e1 = 2" :value="e1 === 2">
                        <span style="color:white; font-family:'vazir'; margin-top:-5px;">مدیریت الگو</span>
                        <v-icon style="color:white; font-size:28pt; margin-top:-12px;">mdi-library-books</v-icon>
                    </v-btn>
                </router-link>
                <router-link to="/AdminPage/UserManagement">
                    <v-btn flat light @click.native="e1 = 3" :value="e1 === 3">
                        <span style="color:white; font-family:'vazir'; margin-top:-8px;">مدیریت کاربر</span>
                        <v-icon style="color:white; font-size:28pt; margin-top:-12px;">mdi-account-multiple</v-icon>
                    </v-btn>
                </router-link>
                <router-link to="/AdminPage">
                    <v-btn flat light @click.native="e1 = 4" :value="e1 === 4">
                        <span style="color:white; font-family:'vazir'; margin-top:-8px;">خانه</span>
                        <v-icon style="color:white; font-size:28pt; margin-top:-12px;">mdi-home</v-icon>
                    </v-btn>
                </router-link>
            </v-bottom-nav>
        </v-card>
        <div style="margin-top:100px; text-align:center;">
            <!-- <div v-if="e1===4" style="width:50%; margin:0 auto;border-radius:20px; height:250px; background-color:#8e5a0d;">
                <h1 style="color:rgb(10, 31, 93); padding-top:100px;">Home Page</h1>
                <button @click="CheckToken">Check Token</button>
            </div> -->
            <router-view></router-view>
        </div>
    </div>
</template>

<script src='./AdminPage.ts'></script>

<style src='./AdminPage.css'>