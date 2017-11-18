<template>
  <div class="wine-listing">
    <div>{{wine.Name}}</div>
    <div>{{wine.Price}}</div>
  </div>
</template>

<script>
export default {
  name: "WineListing",
  props: ["wine"]
};
</script>

<style scoped>
.wine-listing {
  width: 300px;
  height: 100px;
  padding: 5px;
  border: solid;
}
</style>
