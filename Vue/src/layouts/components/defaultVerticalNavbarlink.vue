<script setup>

const props = defineProps({
    drawer:{
        type:Boolean,
        required:null
    }
})

</script>
<template>
    <v-navigation-drawer v-model="$props.drawer"></v-navigation-drawer>
</template>