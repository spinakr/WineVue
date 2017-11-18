<template>
    <div id="inventory-list">
        <wine-list />
    </div>
</template>

<script>
import WineList from './WineList.vue'

export default {
    name: 'WishList',
    components: {
        WineList    
    }
}
</script>
