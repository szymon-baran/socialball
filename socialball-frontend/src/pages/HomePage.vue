<template>
  <div class="home-background">
    <div class="transparent-box">
      <div class="text-inside">
        <h2>Połącz się z innymi na boisku już teraz!</h2>
        <DxButton text="Zaloguj się" @click="routerPushToLogin" />
        <DxButton text="Zarejestruj się" @click="routerPushToRegister" />
        <p>
          Jeśli jesteś zainteresowany przyłączeniem swojej drużyny do programu
          Socialball, skontaktuj się ze wsparciem strony
          <a href="mailto:szymon.w.baran@gmail.com">tutaj</a>.
        </p>
      </div>
    </div>
  </div>
</template>
<script>
import DxButton from "devextreme-vue/button";
import router from "../router";

export default {
  components: {
    DxButton,
  },
  methods: {
    routerPushToLogin() {
      router.push({ path: "/login" });
    },
    routerPushToRegister() {
      router.push({ path: "/register" });
    },
  },
};
</script>

<style></style>
