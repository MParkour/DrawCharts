import { Vue, Component } from 'av-ts';

@Component({
    components: {
        MenuComponent: require('../navmenu/navmenu.vue')
    }
})
export default class AppComponent extends Vue {
}