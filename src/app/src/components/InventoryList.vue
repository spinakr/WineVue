<template>
  <div>
      <h2>InventoryList</h2>
      {{wines}}
  </div>
</template>

<script>
import wineService from '../services/wines';

export default {
  name: 'InventoryList',
  data () {
    return {
        wines: [],
        error: ''
    }
  },

  created () {
    wineService.get('wines')
        .then(response => {
            this.wines = response.data.Wines
        })
        .catch(e => {
            this.error = e
        });
  }
}
</script>

<style scoped>
</style>
