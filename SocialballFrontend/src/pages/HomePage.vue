<template>
  <div class="home-background">
    <div class="main-text">
        <h3 class="mb-4">Połącz się z innymi na boisku już teraz!</h3>
        <DxButton text="Zaloguj się" @click="routerPushToLogin" />
        <span class="ml-3">
          <DxButton
            text="Zarejestruj się"
            @click="routerPushToRegister"
            type="default"
          />
        </span>
        <p class="mt-4">
          Jeśli jesteś zainteresowany przyłączeniem swojej drużyny do programu
          Socialball, skontaktuj się ze wsparciem strony
          <a href="mailto:szymon.w.baran@gmail.com">tutaj</a>.
        </p>
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
      1;
    },
  },
};
</script>

<style></style>
