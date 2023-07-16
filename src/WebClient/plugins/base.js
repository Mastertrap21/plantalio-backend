import Vue from 'vue';
import BaseCard from '@/components/base/Card';
import GuestHub from '~/plugins/guest.hub';

Vue.component(BaseCard.name, BaseCard);
Vue.use(GuestHub);
