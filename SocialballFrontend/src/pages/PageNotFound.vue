<template>
  <div class="text-center big-data-grid" id="not-found-container">
    <i class="far fa-frown main-color mt-3"></i>
    <h1 class="m-4">404</h1>
    <h3>Nie znaleziono wybranej strony.</h3>
    <DxButton
      text="Powrót na stronę główną"
      type="default"
      @click="routerPushToHome"
    />
  </div>
</template>
<script>
import DxButton from "devextreme-vue/button";
import router from "../router";

export default {
  name: "PageNotFound",
  methods: {
    routerPushToHome() {
      router.push({ path: "/" });
    },
  },
  components: {
    DxButton,
  },
};
</script>
<style scoped>
.fa-frown {
  font-size: 600%;
}
</style>
