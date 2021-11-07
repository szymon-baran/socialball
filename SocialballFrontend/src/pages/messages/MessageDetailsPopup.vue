<template>
  <div>
    <DxPopup
      v-model:visible="popupVisible"
      :drag-enabled="false"
      :close-on-outside-click="false"
      :show-close-button="false"
      :show-title="true"
      :width="600"
      :height="700"
      container=".dx-viewport"
      :title="message.message.subject"
      :shading="false"
      v-if="message"
    >
      <DxToolbarItem
        widget="dxButton"
        toolbar="bottom"
        location="after"
        :options="closeButtonOptions"
      />
      <div>
        <span v-html="message.message.content"></span>
      </div>
    </DxPopup>
  </div>
</template>
<script>
import { DxPopup, DxToolbarItem } from "devextreme-vue/popup";
import { mapActions } from "vuex";

export default {
  name: "MessageDetails",
  props: {
    message: {
      type: Object,
      required: true,
    },
  },
  data() {
    return {
      popupVisible: true,
      closeButtonOptions: {
        text: "Zamknij",
        onClick: () => {
          this.popupVisible = false;
          this.$emit("closed");
        },
      },
    };
  },
  components: {
    DxPopup,
    DxToolbarItem,
  },
  methods: {
    ...mapActions({
      markMessageAsRead: "messages/markMessageAsRead",
    }),
  },
  mounted() {
    this.popupVisible = true;
    if (this.message.isRead === false) {
      this.markMessageAsRead(this.message);
    }
  },
};
</script>
