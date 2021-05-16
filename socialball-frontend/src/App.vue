<template>
  <main-header></main-header>
  <router-view></router-view>
</template>

<script>

import MainHeader from "./components/layout/MainHeader";

export default {
  components: {
    MainHeader,
  },
  created() {
    document.title = "Socialball";
  },
};
</script>

<style>
@import "./assets/styles.css";
</style>
