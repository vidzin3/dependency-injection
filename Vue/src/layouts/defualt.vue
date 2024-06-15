<script setup>
import defaultVerticalNavbarlink from '@/layouts/components/defaultVerticalNavbarlink.vue'
import defaultHorizentalLink from '@/layouts/components/defaultHorizentalLink.vue'
import { ref } from 'vue';

const drawer = ref(null);

</script>
<template>
    <v-app>
        <defaultVerticalNavbarlink :drawer="drawer" @update:drawer="drawer = $event" />
        <defaultHorizentalLink :drawer="drawer" @update:drawer="drawer = $event" />
    </v-app>
</template>