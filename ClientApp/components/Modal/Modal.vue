<template>
    <div class="fade modal" id="ModalForm" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header" style="padding:0px; background-color:rgb(29, 164, 171);">
                    <slot name="ModalHeader"></slot>
                </div>
                <div class="modal-body">
                    <slot name="ModalBody"></slot>
                </div>
                <div class="modal-footer">
                    <slot name="ModalFooter"></slot>
                </div>
            </div>
        </div>
    </div>
</template>

<script src='./Modal.ts'></script>

<style src="./Modal.css" scoped>