<template>
  <div id="inventory-list">
    <wine-list />
  </div>
</template>

<script>
import WineList from "../components/WineList.vue";

export default {
  name: "InventoryList",

  components: {
    WineList
  }
};
</script>
