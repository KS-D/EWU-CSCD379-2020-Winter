import '../styles/site.scss';

import Vue from 'vue';

import GiftsComponent from './components/Gift/GiftsComponent.vue'
import GroupComponent from './components/Group/GroupComponent.vue'
import UserComponent from './components/User/UsersComponent.vue'

document.addEventListener("DOMContentLoaded", async () => {
    if (document.getElementById('giftList')) {
        new Vue({
            render: h => h(GiftsComponent)
        }).$mount('#giftList');
    }
    if (document.getElementById('groupList')){
        new Vue({
            render: h => h(GroupComponent)
        }).$mount('#groupList');
    }
    if (document.getElementById('userList')){
        new Vue({
            render: h => h(UserComponent)
        }).$mount('#userList');
    }
});