<template>
  <div class="row">
    <DxButton
      text="Wyślij wiadomość do drużyny"
      @click="showAddTeamMessagePopup"
      type="default"
    />
  </div>
  <TeamMessageAdd
    :isAddTeamMessagePopupVisible="isAddTeamMessagePopupVisible"
    v-if="isAddTeamMessagePopupVisible"
  />
</template>
<script>
import DxButton from "devextreme-vue/button";
import TeamMessageAdd from "./TeamMessageAdd.vue";

export default {
  name: "TeamMessagesList",
  data() {
    return {
      isAddTeamMessagePopupVisible: false,
    };
  },
  methods: {
    showAddTeamMessagePopup() {
      this.isAddTeamMessagePopupVisible = true;
    },
  },
  components: {
    DxButton,
    TeamMessageAdd,
  },
};
</script>
