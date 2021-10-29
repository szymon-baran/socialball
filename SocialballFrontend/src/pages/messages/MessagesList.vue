<template>
  <div class="big-data-grid">
    <div class="row">
      <div class="col">
        <h3>Wiadomości</h3>
      </div>
      <div class="col text-right">
        <DxButton
          text="Wyślij wiadomość do drużyny"
          @click="showAddTeamMessagePopup"
          type="default"
        />
      </div>
    </div>
    <DxDataGrid
      :data-source="getMessages"
      :remote-operations="false"
      :row-alternation-enabled="true"
      :show-borders="true"
      :hover-state-enabled="true"
      @row-click="showMessageDetails"
    >
      <DxFilterRow :visible="true" />
      <DxLoadPanel :enabled="true" />
      <DxColumn data-field="subject" caption="Tytuł wiadomości" />
      <DxColumn
        data-field="sentOn"
        caption="Data otrzymania wiadomości"
        data-type="datetime"
        format="dd/MM/yyyy HH:mm"
        :editorOptions="{ showClearButton: true }"
      />
      <DxColumn data-field="messageType" caption="Typ wiadomości">
        <DxLookup
          :data-source="messageTypes"
          value-expr="value"
          display-expr="name"
        />
      </DxColumn>
    </DxDataGrid>
  </div>
  <TeamMessageAdd
    v-if="isAddTeamMessagePopupVisible"
    @close="onAddTeamMessagePopupClose"
  />
  <MessageDetailsPopup
    :message="detailsPopupOptions.selectedMessage"
    v-if="detailsPopupOptions.isVisible"
    @closed="onPopupClosed()"
  />
</template>
<script>
import DxButton from "devextreme-vue/button";
import { mapActions, mapGetters } from "vuex";
import {
  DxDataGrid,
  DxLoadPanel,
  DxColumn,
  DxFilterRow,
  DxLookup,
} from "devextreme-vue/data-grid";
import TeamMessageAdd from "./TeamMessageAdd.vue";
import MessageDetailsPopup from "./MessageDetailsPopup.vue";

export default {
  name: "MessagesList",
  data() {
    return {
      isAddTeamMessagePopupVisible: false,
      detailsPopupOptions: {
        isVisible: false,
        selectedMessage: {},
      },
      messageTypes: [],
    };
  },
  computed: {
    ...mapGetters({
      getLoggedInUser: "authentication/getLoggedInUser",
      getMessages: "messages/getMessages",
    }),
  },
  methods: {
    ...mapActions({
      setMessages: "messages/setMessages",
      setMessageTypesToLookup: "messages/setMessageTypesToLookup",
    }),
    showAddTeamMessagePopup() {
      this.isAddTeamMessagePopupVisible = true;
    },
    onAddTeamMessagePopupClose() {
      this.isAddTeamMessagePopupVisible = false;
    },
    showMessageDetails(e) {
      this.detailsPopupOptions.isVisible = true;
      this.detailsPopupOptions.selectedMessage = e.data;
    },
    onPopupClosed() {
      this.detailsPopupOptions.isVisible = false;
      this.detailsPopupOptions.selectedMessage = {};
    },
  },
  components: {
    DxButton,
    DxDataGrid,
    DxLoadPanel,
    DxColumn,
    DxFilterRow,
    DxLookup,
    TeamMessageAdd,
    MessageDetailsPopup,
  },
  mounted() {
    this.setMessageTypesToLookup().then((response) => {
      this.messageTypes = response.data;
    });
    this.setMessages(this.getLoggedInUser.id);
  },
};
</script>
<style scoped>
h4 {
  font-weight: bolder;
}
</style>
