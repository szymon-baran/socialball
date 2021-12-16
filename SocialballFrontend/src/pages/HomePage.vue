<template>
  <div class="home-background">
    <div class="main-text">
      <div class="glass">
        <h3 class="mb-4">Połącz się z innymi na boisku już teraz!</h3>
        <div class="row">
          <div class="col mt-2">
            <DxButton
              text="Dołącz jako zawodnik"
              @click="routerPushToRegister"
              type="default"
            />
          </div>
          <div class="col mt-2">
            <DxButton
              text="Dodaj swoją drużynę"
              @click="routerPushToTeamAdd"
              type="normal"
            />
          </div>
        </div>
        <div class="row mt-5">
          <p>
            W przypadku wystąpienia problemów z funkcjonowaniem serwisu, zaleca
            się kontakt ze wsparciem
            <a href="mailto:szymon.w.baran@gmail.com">tutaj</a>.
          </p>
        </div>
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
    routerPushToRegister() {
      router.push({ path: "/register" });
    },
    routerPushToTeamAdd() {
      router.push({ path: "/add-team" });
    },
  },
};
</script>
