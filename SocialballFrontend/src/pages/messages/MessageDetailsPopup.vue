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
      :title="message.subject"
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
        <span v-html="message.content"></span>
      </div>
    </DxPopup>
  </div>
</template>
<script>
import { DxPopup, DxToolbarItem } from "devextreme-vue/popup";

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
  mounted() {
    this.popupVisible = true;
  },
};
</script>
