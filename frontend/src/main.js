import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
//import './index.css' // Tailwind
import "@fortawesome/fontawesome-free/css/all.css"
import "./assets/base.css";

createApp(App)
  .use(router)
  .mount('#app')
