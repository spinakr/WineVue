<template>
    <div id="inventory-list">
        <div v-if="error">
            {{error}}
        </div>
        <wine-list :wines="wines" />
    </div>
</template>

<script>
import wineService from '../services/wines';
import WineList from './WineList.vue'

export default {
    name: 'InventoryList',
    data () {
        return {
            wines: [],
            error: ''
        }
    },

    created () {
        wineService.get('inventory')
            .then(response => {
                this.wines = response.data.Wines
            })
            .catch(e => {
                this.error = e
            });
    },
    components: {
        WineList    
    }
}
</script>
