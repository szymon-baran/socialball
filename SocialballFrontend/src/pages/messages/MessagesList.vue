<template>
  <div class="big-data-grid">
    <div class="row">
      <div class="col">
        <h3>Wiadomości</h3>
      </div>
      <div class="col text-right">
        <DxButton
          text="Wyślij wiadomość do swojej drużyny"
          @click="showAddMessagePopup(messageTypeEnum.TEAM)"
          type="default"
          styling-mode="outlined"
          class="mr-2"
          v-if="getLoggedInUser.userType === userTypeEnum.TEAM_MANAGEMENT"
        />
        <DxButton
          text="Wyślij wiadomość prywatną"
          @click="showAddMessagePopup(messageTypeEnum.PRIVATE)"
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
      <DxSorting mode="none" />
      <DxColumn data-field="message.subject" caption="Tytuł wiadomości" />
      <DxColumn
        data-field="message.sentOn"
        caption="Data otrzymania wiadomości"
        data-type="datetime"
        format="dd/MM/yyyy HH:mm"
        :editorOptions="{ showClearButton: true }"
        :sort-index="1"
        sort-order="desc"
      />
      <DxColumn data-field="message.messageType" caption="Typ wiadomości">
        <DxLookup
          :data-source="messageTypes"
          value-expr="value"
          display-expr="name"
        />
      </DxColumn>
      <DxColumn
        data-field="isRead"
        caption="Czy odczytana?"
        :sort-index="0"
        sort-order="asc"
      />
    </DxDataGrid>
  </div>
  <MessageAdd
    v-if="addMessagePopupOptions.isVisible"
    :messageType="addMessagePopupOptions.messageType"
    @close="onAddMessagePopupClose"
  />
  <MessageDetailsPopup
    :message="detailsPopupOptions.selectedMessage"
    v-if="detailsPopupOptions.isVisible"
    @closed="onDetailsPopupClose"
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
  DxSorting,
} from "devextreme-vue/data-grid";
import MessageAdd from "./MessageAdd.vue";
import MessageDetailsPopup from "./MessageDetailsPopup.vue";
import { messageTypeEnum } from "../../enums/messageTypeEnum";
import { userTypeEnum } from "../../enums/userTypeEnum";

export default {
  name: "MessagesList",
  data() {
    return {
      messageTypeEnum,
      userTypeEnum,
      addMessagePopupOptions: {
        isVisible: false,
        messageType: null,
      },
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
    showAddMessagePopup(messageType) {
      this.addMessagePopupOptions.messageType = messageType;
      this.addMessagePopupOptions.isVisible = true;
    },
    onAddMessagePopupClose() {
      this.addMessagePopupOptions.messageType = null;
      this.addMessagePopupOptions.isVisible = false;
    },
    showMessageDetails(e) {
      this.detailsPopupOptions.isVisible = true;
      this.detailsPopupOptions.selectedMessage = e.data;
    },
    onDetailsPopupClose() {
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
    DxSorting,
    MessageAdd,
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
