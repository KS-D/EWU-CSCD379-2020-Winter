<template>
    <div>
        <button class="button" @click="createGroup()">Create New</button>
        <table class="table">
            <thead>
                <tr>
                    <th>Title</th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="group in groups" :id="group.id">
                    <td>{{group.title}}</td>
                    <td>
                        <button class="button" @click='setGroup(group)'>Edit</button>
                        <button class="button" @click='deleteGroup(group)'>Delete</button>
                    </td>
                </tr>
            </tbody>
        </table>
        <groups-details-component v-if="selectedGroup != null"
                                  :group="selectedGroup"
                                  @group-saved="refreshGroups()"></groups-details-component>
    </div>
</template>
<script lang="ts">
    import {Vue, Component } from 'vue-property-decorator'
    import {Group, GroupClient } from '../../secretsanta-client' 
    import groupsDetailsComponent from './groupDetailsComponent.vue'
    @Component({
        components: {
            groupsDetailsComponent
        }
    })
    export default class GroupComponent extends Vue {
        groups : Group[] = null;
        selectedGroup: Group = null;

        async loadGroups(){
            let groupClient = new GroupClient();
            this.groups = await groupClient.getAll(); 
        }

        createGroup(){
            this.selectedGroup = <Group>{};
        }

        async mounted(){
            await this.loadGroups();
        }

        setGroup(group : Group){
            this.selectedGroup = group;
        }

        async refreshGroups(){
            this.selectedGroup = null;
            await this.loadGroups();
        }
        
        async deleteGroup(group : Group){
            let groupClient = new GroupClient();
            if (confirm(`Are you sure you want to delete ${group.title}`)) {
                await groupClient.delete(group.id);
            }

            await this.refreshGroups();
        }
    }
</script>