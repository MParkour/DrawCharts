import 'bootstrap';
import Vue from 'vue';
import VueRouter from 'vue-router';
import Vuetify from 'vuetify';
import '../node_modules/vuetify/dist/vuetify.min.css';
import './Styles/Config.css';

Vue.use(VueRouter);
Vue.use(Vuetify);

const routes = [
    { path: '/', component: require('./components/LoginPage/LoginPage.vue') },
    {
        path: '/AdminPage/:id?', component: require('./components/Admin/adminpage/adminpage.vue')
        , children: [
            { path: '/AdminPage/RoleAllocation', component: require('./components/Admin/RoleAllocation/RoleAllocation.vue') },
            { path: '/AdminPage', component: require('./components/Admin/Home/Home.vue') },
            { path: '/AdminPage/UserManagement', component: require('./components/Admin/UserManagement/UserManagement.vue') },
            { path: '/AdminPage/TemplateManagement', component: require('./components/Admin/TemplateManagement/TemplateManagement.vue') },
        ],
    },
    { path: '/UserPage', component: require('./components/User/UserPage/UserPage.vue') },
    { path: '*', component: require('./components/notfound/notfound.vue') }
];

new Vue({
    el: '#app-root',
    router: new VueRouter({ mode: 'history', routes: routes }),
    render: h => h(require('./components/app/app.vue'))
});
