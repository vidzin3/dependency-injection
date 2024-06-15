<script setup>
const onLogin = () => {
    
}
</script>
<template>
    <v-app>
        <v-main class="h-100">
            <v-row class="h-100" align="center" justify="center">
                <v-col cols="auto">
                    <v-card width="500">
                        <v-card-title>Login</v-card-title>
                        <v-divider></v-divider>
                        <v-card-text>
                            <v-form>
                                <div>
                                    <v-label>Username</v-label>
                                    <v-text-field variant="outlined"></v-text-field>
                                </div>
                                <div>
                                    <v-label>Password</v-label>
                                    <v-text-field variant="outlined"></v-text-field>
                                </div>
                                <div>
                                    <v-btn type="submit">Login</v-btn>
                                </div>
                            </v-form>
                        </v-card-text>
                    </v-card>
                </v-col>
            </v-row>
        </v-main>
    </v-app>
</template>