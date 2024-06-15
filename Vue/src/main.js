import './assets/main.css'

import { createApp } from 'vue'
import App from './App.vue'
import router from './routers/index'
import {createPinia} from 'pinia'

const app = createApp(App)
// Vuetify
import '@mdi/font/css/materialdesignicons.css'
import 'vuetify/styles'
import { createVuetify } from 'vuetify'
import * as components from 'vuetify/components'
import * as directives from 'vuetify/directives'

const vuetify = createVuetify({
  components,
  directives
})

const pinia = createPinia();

app.use(vuetify);
app.use(router);
app.use(pinia);

app.mount('#app')
