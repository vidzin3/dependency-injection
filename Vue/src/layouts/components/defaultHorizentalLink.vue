<script setup>
import { ref, watch } from 'vue';
import {RouterView} from 'vue-router'

const props = defineProps({
    drawer:{
        type:Boolean,
        required:null
    }
})

const emit = defineEmits(['update:drawer']);

</script>
<template>
    <v-app-bar>
        <v-app-bar-nav-icon @click="emit('update:drawer', !props.drawer)"></v-app-bar-nav-icon>
    </v-app-bar>
    <v-main>
        <RouterView />        
    </v-main>
</template>