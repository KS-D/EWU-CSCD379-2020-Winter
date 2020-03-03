<template>
    <div>
        <button class="button" @click="createUser()">Create New</button>
        <table class="table">
            <thead class="tbl-header">
                <tr>
                    <th>First Name</th>
                    <th>Last Name</th>
                    <th></th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="user in users" :id="user.id">
                    <td>{{user.firstName}}</td>
                    <td>{{user.lastName}}</td>
                    <td>
                        <button class="button" @click='setUser(user)'>Edit</button>
                        <button class="button" @click='deleteUser(user)'>Delete</button>
                    </td>
                </tr>
            </tbody>
        </table>
        <users-details-component v-if="selectedUser != null"
                                  :user="selectedUser"
                                  @user-saved="refreshUsers()"></users-details-component>
    </div>
</template>
<script lang="ts">
    import {Vue, Component } from 'vue-property-decorator'
    import {User, UserClient } from '../../secretsanta-client' 
    import UsersDetailsComponent from './usersDetailComponent.vue'
    @Component({
        components: {
            UsersDetailsComponent
        }
    })
    export default class UsersComponent extends Vue {
        users : User[] = null;
        selectedUser: User = null;

        async loadUsers(){
            let userClient = new UserClient;
            this.users = await userClient.getAll(); 
            console.log('hello world');
        }

        createUser(){
            this.selectedUser = <User>{};
        }

        async mounted(){
            await this.loadUsers();
        }

        setUser(user : User){
            this.selectedUser = user;
        }

        async refreshUsers(){
            this.selectedUser = null;
            await this.loadUsers();
        }
        
        async deleteUser(user : User){
            let userClient = new UserClient();
            if (confirm(`Are you sure you want to delete ${user.firstName} ${user.lastName}`)) {
                await userClient.delete(user.id);
            }

            await this.refreshUsers();
        }
    }








</script>