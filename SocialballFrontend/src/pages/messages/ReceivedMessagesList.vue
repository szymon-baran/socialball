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
      <DxColumn data-field="message.subject" caption="Tytuł" />
      <DxColumn
        data-field="message.fromUser.userData.lastName"
        caption="Nadawca"
      />
        <!-- :calculate-display-value="getFullName" -->
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
  <MessageDetailsPopup
    :message="detailsPopupOptions.selectedMessage"
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
  name: "ReceivedMessagesList",
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
    // getFullName(rowData) {
    //   return rowData.message.fromUser.userData.firstName + " " + rowData.message.fromUser.userData.lastName;
    // },
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
