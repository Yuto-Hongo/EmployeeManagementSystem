import * as yup from "yup";

// パスワード変更時バリデーション適用
export const passwordSchema = yup.object({
  currentPassword: yup.string().required("現在のパスワードを入力してください"),
  newPassword: yup
    .string()
    .min(
      8,
      "新しいパスワードは８文字以上かつ大文字・小文字のアルファベット,数字を含めた値に設定してください"
    )
    .required("新しいパスワードを入力してください"),
  confirmPassword: yup
    .string()
    .oneOf([yup.ref("newPassword")], "新しいパスワードが一致しません")
    .required("確認用パスワードを入力してください"),
});

// 新規登録時バリデーション適用
export const passwordRegisterSchema = yup.object({
  password: yup
    .string()
    .required("パスワードは必須です")
    .matches(
      /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[A-Za-z\d]{8,}$/,
      "パスワードは8文字以上で、大文字・小文字・数字を含めてください"
    ),
  confirmPassword: yup
    .string()
    .oneOf([yup.ref("password")], "パスワードが一致しません")
    .required("確認用パスワードを入力してください"),
});
