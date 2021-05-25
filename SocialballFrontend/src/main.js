import { createApp } from "vue";
import App from "./App.vue";
import router from "./router";
import store from "./store/index";
import Toast, { POSITION } from "vue-toastification";
import "vue-toastification/dist/index.css";
import "devextreme/dist/css/dx.common.css";
import "devextreme/dist/css/dx.material.teal.dark.compact.css";
import "bootstrap/dist/css/bootstrap.min.css";
import "bootstrap";
import "./assets/styles.css";

const app = createApp(App);

app.use(router);
app.use(store);
app.use(Toast, {
  position: POSITION.BOTTOM_RIGHT,
});

app.mount("#app");
