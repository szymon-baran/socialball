<template>
  <main-header></main-header>
  <DxLoadPanel
    v-model:visible="loadingVisible"
    :on-shown="onShown"
    :shading="true"
    shading-color="rgba(0,0,0,0.2)"
  />
  <router-view></router-view>
</template>

<script>
import MainHeader from "./components/layout/MainHeader";
import { DxLoadPanel } from "devextreme-vue/load-panel";

export default {
  data() {
    return {
      loadingVisible: false,
    };
  },
  watch: {
    $route() {
      this.loadingVisible = true;
    },
  },
  components: {
    MainHeader,
    DxLoadPanel,
  },
  methods: {
    onShown() {
      setTimeout(() => {
        this.loadingVisible = false;
      }, 1000);
    },
  },
  created() {
    document.title = "Socialball";
  },
};
</script>

<style></style>
