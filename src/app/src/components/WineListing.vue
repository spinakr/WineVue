<template>
  <div class="wine-listing">
    <div class="left-column">
      <div class="wine-prop"> {{wine.Name}}</div>
      <div class="wine-prop">{{wine.Fruit}}</div>
      <div class="wine-prop">{{wine.Price}}</div>
    </div>
    <div class="right-column">
      <div>
        <img v-bind:src="wineIconPath" height="50" width="50">
      </div>
      <div>
        <img v-bind:src="flagPath" height="50" width="50">
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: "WineListing",
  props: ["wine"],
  data() {
    return {
      wineIconPath: `/images/${this.wine.Type}.svg`,
      flagPath: `/images/${this.wine.Country}.svg`
    };
  }
};
</script>

<style scoped>
.wine-listing {
  width: 300px;
  height: 100px;
  padding: 5px;
  border: solid;
}

.left-column {
  float: left;
  width: 80%;
}

.right-column {
  clear: right;
}

.wine-prop {
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
}
</style>
