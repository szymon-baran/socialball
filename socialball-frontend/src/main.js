import 'devextreme/dist/css/dx.common.css';
import 'devextreme/dist/css/dx.light.css';
import { createApp } from 'vue'
import App from './App.vue'
import router from './router';
import store from './store/index';
import 'devextreme/dist/css/dx.common.css';
import 'devextreme/dist/css/dx.dark.css';

const app = createApp(App);

app.use(router);
app.use(store);

app.mount('#app');
