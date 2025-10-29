import {
  ErrorComponentProps,
  isRedirect,
  useNavigate,
} from "@tanstack/react-router";

export default function DefaultErrorComponent(props: ErrorComponentProps) {
  const navigate = useNavigate();
  const { error } = props;

  if (error && isRedirect(error)) {
    navigate(error);
  } else {
    throw error;
  }
}
