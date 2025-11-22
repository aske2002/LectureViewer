import Image from "@tiptap/extension-image";

const UpdatedImage = Image.extend({
  name: "image",
  addAttributes() {
    return {
      width: {
        default: null,
      },
      height: {
        default: null,
      },
    };
  },
});

export default UpdatedImage;
