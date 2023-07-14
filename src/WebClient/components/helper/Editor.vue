<template>
    <ckeditor v-model="data" :editor="editor" :config="config"></ckeditor>
</template>
<script>
// import ClassicEditor from "@ckeditor/ckeditor5-build-classic";

let ClassicEditor;
if (process.client) {
    ClassicEditor = require('@ckeditor/ckeditor5-build-classic');
}

export default {
    props: {
        value: {
            default: '',
        },
        config: {
            // The configuration of the editor.
        },
    },
    data() {
        return {
            editor: ClassicEditor,
            editorData: '',
        };
    },
    computed: {
        data: {
            set(value) {
                this.editorData = value;
                this.$emit('input', value);
            },
            get() {
                return this.editorData;
            },
        },
    },
    created() {
        this.editorData = this.value;
    },
};
</script>
