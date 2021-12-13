<template>
  <div style="min-height: 70vh;">
    <DxDataGrid
      :data-source="getMessages"
      :remote-operations="false"
      :row-alternation-enabled="true"
      :show-borders="true"
      :hover-state-enabled="true"
      @row-click="showMessageDetails"
      :column-auto-width="true"
      width="100%"
    >
      <DxFilterRow :visible="true" />
      <DxLoadPanel :enabled="true" />
      <DxSorting mode="none" />
      <DxColumn data-field="message.subject" caption="Tytuł wiadomości" />
      <DxColumn data-field="toUser.userData.lastName" caption="Odbiorca" />
      <DxColumn
        data-field="sentOn"
        caption="Data wysłania wiadomości"
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
    </DxDataGrid>
  </div>
  <MessageDetailsPopup
    :message="detailsPopupOptions.selectedMessage"
    :isSentMessage="true"
    v-if="detailsPopupOptions.isVisible"
    @closed="onDetailsPopupClose"
  />
</template>
<script>
import { mapActions, mapGetters } from "vuex";
import {
  DxDataGrid,
  DxLoadPanel,
  DxColumn,
  DxFilterRow,
  DxLookup,
  DxSorting,
} from "devextreme-vue/data-grid";
import MessageDetailsPopup from "./MessageDetailsPopup.vue";

export default {
  name: "SentMessagesList",
  data() {
    return {
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
      setMessageTypesToLookup: "messages/setMessageTypesToLookup",
    }),
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
    DxDataGrid,
    DxLoadPanel,
    DxColumn,
    DxFilterRow,
    DxLookup,
    DxSorting,
    MessageDetailsPopup,
  },
  mounted() {
    this.setMessageTypesToLookup().then((response) => {
      this.messageTypes = response.data;
    });
  },
};
</script>
