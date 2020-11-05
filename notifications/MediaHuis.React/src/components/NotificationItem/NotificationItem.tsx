import React from "react";
import S from "./NotificationItem.style";
import { Notification } from "../../types/Notification";

interface Props {
  notification: Notification;
  selected?: boolean;
  onClick?: () => void;
}

const NotificationItem: React.FC<Props> = ({
  notification,
  onClick,
  selected,
}) => {
  return (
    <S.Root onClick={onClick} selected={selected}>
      <S.Title selected={selected}>{notification.title}</S.Title>
      <S.Body selected={selected}>{notification.body}</S.Body>
    </S.Root>
  );
};

export default NotificationItem;
